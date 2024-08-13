using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Blog.PublicApi.Infrastructure.Paging;

[DataContract]
public class SortCriteria<T>
{
    public SortCriteria(string sortColumn)
    {
        this.SortDirection = SortDirection.Ascending;
        this.SortColumn = sortColumn;
    }

    public SortCriteria(Expression<Func<T, object>> expression)
    {
        this.SortDirection = SortDirection.Ascending;
        this.SortColumn = PropertyResolver.GetPropertyName(expression);
    }

    public SortCriteria(string sortColumn, SortDirection sortDirection)
    {
        this.SortDirection = sortDirection;
        this.SortColumn = sortColumn;
    }

    public SortCriteria(Expression<Func<T, object>> expression, SortDirection sortDirection)
    {
        this.SortDirection = sortDirection;
        this.SortColumn = PropertyResolver.GetPropertyName(expression);
    }
    
    [DataMember]
    public SortDirection SortDirection { get; set; }
    
    [DataMember]
    public string SortColumn { get; set; }
    
    public void SetSortExpression(Expression<Func<T, object>> expression)
    {
        SortColumn = PropertyResolver.GetPropertyName(expression);
    }
    
    public override string ToString()
    {
        return $"SortColumn: {SortColumn}";
    }
}