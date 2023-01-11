using System.Collections;
using System.Runtime.Serialization;

namespace MyBlogOnCore.Infrastructure.Paging;

[DataContract]
public class PagedResult<T> : IEnumerable<T>
{
    public PagedResult()
    {
        this.Items = new List<T>();
        this.Paging = new Paging<T>();
    }
    
    public PagedResult(IEnumerable<T> items, int totalNumberOfItems, Paging<T> paging)
    {
        this.Items = items;
        this.TotalNumberOfItems = totalNumberOfItems;
        this.Paging = paging;
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