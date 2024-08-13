using System.Runtime.Serialization;

namespace Blog.PublicApi.Infrastructure.Paging;

[DataContract]
public enum SortDirection
{
    [EnumMember]
    Ascending,

    [EnumMember]
    Descending
}