﻿using System.Threading.Tasks;
using PlatoCore.Abstractions.Extensions;
using PlatoCore.Messaging.Abstractions;
using Plato.Markdown.Services;

namespace Plato.Markdown.Subscribers
{

    public class ParseEntityHtmlSubscriber : IBrokerSubscriber
    {

        private readonly IMarkdownParserFactory _markdownParserFactory;
        private readonly IBroker _broker;

        public ParseEntityHtmlSubscriber(
            IMarkdownParserFactory markdownParserFactory,
            IBroker broker)
        {
            _markdownParserFactory = markdownParserFactory;
            _broker = broker;
        }
        
        public void Subscribe()
        {

            // Add a subscription to convert markdown to html
            _broker.Sub<string>(new MessageOptions()
            {
                Key = "ParseEntityHtml",
            }, async message => await ParseEntityHtmlAsync(message.What));
            
            // Add a subscription to produce a plaintext abstract from our markdown
            _broker.Sub<string>(new MessageOptions()
            {
                Key = "ParseEntityAbstract",
            }, async message => await ParseEntityAbstractAsync(message.What));
            
        }

        public void Unsubscribe()
        {
            _broker.Unsub<string>(new MessageOptions()
            {
                Key = "ParseEntityHtml"
            }, async message => await ParseEntityHtmlAsync(message.What));

            _broker.Unsub<string>(new MessageOptions()
            {
                Key = "ParseEntityAbstract"
            }, async message => await ParseEntityAbstractAsync(message.What));
        }
        
        private async Task<string> ParseEntityHtmlAsync(string markdown)
        {
            var parser = _markdownParserFactory.GetParser();
            return await parser.ParseAsync(markdown);
        }

        private async Task<string> ParseEntityAbstractAsync(string markdown)
        {
            var html = await ParseEntityHtmlAsync(markdown);
            return html.PlainTextulize().TrimToAround(225);
        }
        
    }

}
