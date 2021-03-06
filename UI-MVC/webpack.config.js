const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
	entry: {
		site: './wwwroot/js/entries/site.js',
		validation: './wwwroot/js/entries/validation.js',
		idea: './wwwroot/js/entries/idea.js',
		project: './wwwroot/js/entries/project.js',
		questionnaire: './wwwroot/js/entries/questionnaire.js',
		questionnaire_results: './wwwroot/js/entries/questionnaire_results.js',
		admin: './wwwroot/js/entries/admin.js',
	},
	output: {
		filename: '[name].entry.js',
		path: __dirname + '/wwwroot/dist'
	},
	devtool: 'source-map',
	mode: 'development',
	module: {
		rules: [
			{ test: /\.(sa|sc|c)ss$/, use: [{ loader: MiniCssExtractPlugin.loader }, "css-loader", "sass-loader"] },
			{ test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, loader: "file-loader" },
			{ test: /\.(woff|woff2)$/, loader:"url-loader?prefix=font/&limit=5000" },
			{ test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/, loader: "url-loader?limit=10000&mimetype=application/octet-stream" },
			{ test: /\.svg(\?v=\d+\.\d+\.\d+)?$/, loader: "url-loader?limit=10000&mimetype=image/svg+xml" }
		]
	},
	plugins: [
		new MiniCssExtractPlugin({
			filename: "[name].css"
		})
	]
};
