using HtmlAgilityPack;
using KnowledgeSharingApi.Infrastructures.Interfaces.Markdown;
using Markdig;


namespace KnowledgeSharingApi.Infrastructures.Markdown
{
    public class MarkdownConverter : IMarkdownConverter
    {
        public string GetPureText(string content)
        {
            return RemoveMarkdownSyntax(content);
        }

        public string RemoveHtmlTag(string content)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            return doc.DocumentNode.InnerText;
            
        }

        public string RemoveMarkdownSyntax(string content)
        {
            var pipeline = new MarkdownPipelineBuilder().Build();
            var html = Markdig.Markdown.ToHtml(content, pipeline);
            return RemoveHtmlTag(html);
        }
    }
}
