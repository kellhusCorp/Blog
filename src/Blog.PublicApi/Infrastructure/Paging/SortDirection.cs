using System.Runtime.Serialization;

namespace MyBlogOnCore.Infrastructure.Paging;

[DataContract]
public enum SortDirection
{
    [EnumMember]
    Ascending,

    [EnumMember]
    Descending
}