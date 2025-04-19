using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Shared.DTOs.Responses;
using KMN_Tontine.Shared.Options;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Linq;

namespace KMN_Tontine.Application.Services
{
    public class EmailCheckerService : IEmailCheckerService
    {
        private readonly ILogger<EmailCheckerService> _logger;
        private readonly EmailCheckerOptions _options;
        private ImapClient _client;

        public EmailCheckerService(
            IOptions<EmailCheckerOptions> options,
            ILogger<EmailCheckerService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetTransfersAsync()
        {
            try
            {
                await ConnectAsync();

                var folder = await _client.GetFolderAsync(_options.EmailFolder);
                await folder.OpenAsync(FolderAccess.ReadOnly);

                var query = SearchQuery.SubjectContains("transfer").Or(SearchQuery.SubjectContains("payment"));
                var uids = await folder.SearchAsync(query);

                return uids.Select(uid => uid.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transfers");
                throw;
            }
            finally
            {
                await DisconnectAsync();
            }
        }

        public async Task<TransferInfo> GetTransferDetailsAsync(string transferId)
        {
            try
            {
                await ConnectAsync();

                var folder = await _client.GetFolderAsync(_options.EmailFolder);
                await folder.OpenAsync(FolderAccess.ReadOnly);

                var message = await folder.GetMessageAsync(new UniqueId(uint.Parse(transferId)));

                var amount = ExtractAmount(message.TextBody);
                var senderName = message.From[0].Name;
                var promiseId = ExtractPromiseId(message.TextBody);

                return new TransferInfo
                {
                    Amount = amount,
                    SenderName = senderName,
                    PromiseId = promiseId.HasValue ? (int?)promiseId.Value.GetHashCode() : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transfer details");
                throw;
            }
            finally
            {
                await DisconnectAsync();
            }
        }

        public async Task CheckEmailsAsync()
        {
            try
            {
                var transfers = await GetTransfersAsync();
                foreach (var transferId in transfers)
                {
                    var transferDetails = await GetTransferDetailsAsync(transferId);
                    _logger.LogInformation($"Found transfer: Amount={transferDetails.Amount}, Sender={transferDetails.SenderName}, PromiseId={transferDetails.PromiseId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking emails");
                throw;
            }
        }

        private async Task ConnectAsync()
        {
            _client = new ImapClient();
            await _client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl);
            await _client.AuthenticateAsync(_options.Username, _options.Password);
        }

        private async Task DisconnectAsync()
        {
            if (_client != null && _client.IsConnected)
            {
                await _client.DisconnectAsync(true);
                _client.Dispose();
            }
        }

        private decimal ExtractAmount(string text)
        {
            var match = Regex.Match(text, @"Amount:\s*(\d+(\.\d{2})?)");
            if (match.Success)
            {
                return decimal.Parse(match.Groups[1].Value);
            }
            return 0;
        }

        private Guid? ExtractPromiseId(string text)
        {
            var match = Regex.Match(text, @"PromiseId:\s*([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})");
            if (match.Success)
            {
                return Guid.Parse(match.Groups[1].Value);
            }
            return null;
        }
    }
} 