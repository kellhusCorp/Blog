using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace MyBlogOnCore.Infrastructure.Paging;

[DataContract]
public class Paging<T>
{
    public Paging()
        : this(0, 20)
    {
    }
    
    public Paging(int skip, int top)
    {
        this.SortDirection = SortDirection.Ascending;
        this.Skip = skip;
        this.Top = top;
    }
    
    public Paging(int skip, int top, string sortColumn)
        : this(skip, top)
    {
        this.SortColumn = sortColumn;
    }
    
    public Paging(int skip, int top, string sortColumn, SortDirection sortDirection)
        : this(skip, top, sortColumn)
    {
        this.SortDirection = sortDirection;
    }
    
    [DataMember]
    public SortDirection SortDirection { get; set; }
    
    [DataMember]
    public string? SortColumn { get; set; }

    [DataMember]
    public List<SortCriteria<T>> AdditionalSortCriteria { get; } = new List<SortCriteria<T>>();
    
    [DataMember]
    public int Skip { get; set; }
    
    [DataMember]
    public int Top { get; set; }
    
    public void SetSortExpression(Expression<Func<T, object>> expression)
    {
        this.SortColumn = PropertyResolver.GetPropertyName(expression);
    }
    
    public override string ToString()
    {
        return $"SortColumn: {SortColumn}, Top: {Top}, Skip: {Skip}";
    }
}