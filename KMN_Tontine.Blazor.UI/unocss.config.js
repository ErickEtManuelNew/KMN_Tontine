import { defineConfig, presetWind } from 'unocss'

export default defineConfig({
    presets: [presetWind()],
    content: ["./**/*.razor", "./wwwroot/index.html"]
})
});