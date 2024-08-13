using System.Collections;
using System.Runtime.Serialization;

namespace Blog.PublicApi.Infrastructure.Paging;

[DataContract]
public class PagedResult<T> : IEnumerable<T>
{
    public PagedResult()
    {
        Items = new List<T>();
        Paging = new Paging<T>();
    }
    
    public PagedResult(IEnumerable<T> items, int totalNumberOfItems, Paging<T> paging)
    {
        Items = items;
        TotalNumberOfItems = totalNumberOfItems;
        Paging = paging;
    }

    [DataMember]
    public IEnumerable<T> Items { get; private set; }

    public Paging<T> Paging { get; private set; }
    
    [DataMember]
    public int TotalNumberOfItems { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }
}