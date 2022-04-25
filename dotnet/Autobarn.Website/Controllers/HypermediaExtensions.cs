using System.Dynamic;

public class Hal {
    public static dynamic Paginate(string url, 
    int index,
    int count,
    int total
    ) {       			
        dynamic links = new ExpandoObject();
        if (index > 0) {
            links.previous = new { href = $"/api/vehicles?index={index-count}" };
            links.first = new { href = $"/api/vehicles?index=0" };
        }
        if (index < total) {
            links.next = new { href = $"/api/vehicles?index={index+count}" };
            links.final =  new { href = $"/api/vehicles?index={total - total % count}" };
        }
        return links;
    }
}