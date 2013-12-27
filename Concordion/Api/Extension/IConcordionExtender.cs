using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Extension
{
    public interface IConcordionExtender
    {
        /**
         * <summary>
         * Embeds the given JavaScript in the Concordion output.
         * </summary>
         * <param name=javaScript></param>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithEmbeddedJavaScript(string javaScript);

        /**
         * <summary>
         * Copies the given JavaScript file to the Concordion output folder, and adds a link to the JavaScript in the &lt;head&gt; section of the Concordion HTML.  
         * </summary>
         * <param name=jsPath></param>
         * <param name=targetResource></param>
         * <returns>this - to enable fluent interfaces</returns>
         */
        IConcordionExtender WithLinkedJavaScript(string jsPath, Resource targetResource);
    }
}
