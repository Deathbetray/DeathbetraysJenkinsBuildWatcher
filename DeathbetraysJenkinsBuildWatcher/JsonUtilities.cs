using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeathbetraysJenkinsBuildWatcher
{
	static public class JsonUtilities
	{
		// TODO (EdW): Maybe convert to a class and search using the index operator?
		/// <summary>
		/// Finds an element of a certain id within a json element.
		/// </summary>
		/// <param name="_element">The block of json.</param>
		/// <param name="_id">The id of the element to search for.</param>
		/// <param name="_recursive">If true, will search all elements instead of just immediate children.</param>
		/// <returns>The contents of the element specified by the id.</returns>
		static public string Find(JsonElement _element, string _id, bool _recursive = false)
		{
			if (_element.ValueKind == JsonValueKind.Object)
			{
				foreach (var item in _element.EnumerateObject())
				{
					if (_recursive && item.Value.ValueKind == JsonValueKind.Object)
					{
						Find(item.Value, _id, _recursive);
					}

					//if (item.Value.ValueKind == JsonValueKind.Number && item.Name == "Id")
					if (item.Name == _id)
					{
						//Console.WriteLine(item.Value.GetRawText());
						//list.Add(new KeyValuePair<string, string>(item.Name, item.Value.GetRawText()));
						return item.Value.GetRawText();
					}
				}
			}

			return null;
		}

		/// <summary>
		/// "Pretty-fies" a block of json to include new lines and indentation so it's easily
		/// human-readable.
		/// </summary>
		/// <param name="unPrettyJson">Unformatted json.</param>
		/// <returns>The parameter with new lines and indentations included.</returns>
		static public string PrettyJson(string unPrettyJson)
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true
			};

			var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

			return JsonSerializer.Serialize(jsonElement, options);
		}
	}
}
