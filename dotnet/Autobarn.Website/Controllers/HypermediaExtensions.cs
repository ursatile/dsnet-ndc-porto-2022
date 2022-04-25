using Autobarn.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

public static class Hal {
    public static dynamic Paginate(string url,
    int index,
    int count,
    int total
    ) {
        dynamic links = new ExpandoObject();
        if (index > 0) {
            links.previous = new { href = $"/api/vehicles?index={index - count}" };
            links.first = new { href = $"/api/vehicles?index=0" };
        }
        if (index < total) {
            links.next = new { href = $"/api/vehicles?index={index + count}" };
            links.final = new { href = $"/api/vehicles?index={total - total % count}" };
        }
        return links;
    }

    public static dynamic ToDynamic(this object value) {
        IDictionary<string, object> expando = new ExpandoObject();
        var properties = TypeDescriptor.GetProperties(value.GetType());
        foreach (PropertyDescriptor prop in properties) {
            if (Ignore(prop)) continue;
            expando.Add(prop.Name, prop.GetValue(value));
        }
        return expando;
    }

    public static bool Ignore(PropertyDescriptor prop) {
        return prop.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();
    }

    public static dynamic ToResource(this Vehicle v) {
        var result = v.ToDynamic();
        result._links = new {
            self = new {
                href = $"/api/vehicles/{v.Registration}"
            },
            model = new {
                href = $"/api/models/{v.ModelCode}"
            }
        };
        return result;
    }
}