using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api.Listener;

namespace Concordion.Api.Extension
{
    public interface IConcordionExtender
    {
        /**
         * <summary>
         * Adds a command to Concordion.
         * </summary>
         * <param name="namespaceUri">the URI to be used for the namespace of the command.  Must not be <code>concordion.org</code>.</param>
         * <param name="commandName">the name to be used for the command.  The fully qualified name composed of the <code>namespaceURI</code> and
         * <code>commandName</code> must be used to reference the command in the Concordion specification.</param>
         * <param name="command">the command to be executed</param>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithCommand(string namespaceUri, string commandName, ICommand command);

        /**
         * <summary>
         * Adds a listener that is invoked when a Concordion instance is built, providing access to the {@link Target}
         * to which resources can be written.  
         * </summary>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithBuildListener(IConcordionBuildListener listener);

        /**
         * <summary>
         * Copies a resource to the Concordion output.
         * </summary>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithResource(String sourcePath, Resource targetResource);

        /**
         * <summary>
         * Embeds the given CSS in the Concordion output.
         * </summary>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithEmbeddedCss(string css);

        /**
         * <summary>
         * Copies the given CSS file to the Concordion output folder, and adds a link to the CSS in the &lt;head&gt; section of the Concordion HTML.
         * </summary>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithLinkedCss(string cssPath, Resource targetResource);

        /**
         * <summary>
         * Embeds the given JavaScript in the Concordion output.
         * </summary>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithEmbeddedJavaScript(string javaScript);

        /**
         * <summary>
         * Copies the given JavaScript file to the Concordion output folder, and adds a link to the JavaScript in the &lt;head&gt; section of the Concordion HTML.  
         * </summary>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithLinkedJavaScript(string jsPath, Resource targetResource);
    }
}
