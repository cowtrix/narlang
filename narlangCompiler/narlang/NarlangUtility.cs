using System.Linq;
using System.Text.RegularExpressions;

namespace narlang
{
	internal static class NarlangUtility
	{
		internal static string ReplaceVariables(string templateValue, IRenderer renderer, NarlangNode context)
		{
			var match = Regex.Match(templateValue, Const.VARIABLE_REGEX);
			while (match.Success)
			{
				var name = match.Groups[1].Value;
				INarlangObject variableObj = null;
				string stringToInsert = "";
				if (name == Const.NAME_VARIABLE)
				{
					stringToInsert = context.ID.Identifier;
				}
				else if (name == Const.GUID_VARIABLE)
				{
					stringToInsert = context.Guid.ToString();
				} 
				else
				{
					if (!context.Data.TryGetValue(name, out variableObj))
					{
						if(name == Const.RENDER_FUNCTION)
						{
							stringToInsert = renderer.RenderSymbol(context);
						}
						else
						{
							throw new ParseException(context.Address, $"Unable to resolve variable {name}");
						}
					}
					else
					{
						stringToInsert = variableObj.Render(renderer);
					}					
				}
				templateValue = templateValue.Substring(0, match.Index) + stringToInsert + templateValue.Substring(match.Index + match.Length);
				match = Regex.Match(templateValue, Const.VARIABLE_REGEX);
			}
			return templateValue;
		}
	}
}
