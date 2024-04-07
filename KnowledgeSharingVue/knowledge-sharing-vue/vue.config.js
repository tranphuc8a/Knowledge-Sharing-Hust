const { defineConfig } = require('@vue/cli-service');
const webpack = require('webpack');

module.exports = defineConfig({
	transpileDependencies: true,
	configureWebpack: {
		devtool: 'source-map',
		plugins: [
			new webpack.DefinePlugin({
				__VUE_OPTIONS_API__: JSON.stringify(true),
				__VUE_PROD_DEVTOOLS__: JSON.stringify(false),
				// Cài đặt cờ tính năng VUE_PROD_HYDRATION_MISMATCH_DETAILS
				__VUE_PROD_HYDRATION_MISMATCH_DETAILS__: 'false'
			})
		],
	},
});