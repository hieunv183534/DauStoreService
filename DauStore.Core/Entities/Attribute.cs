using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    // thông tin bắt buộc nhập
    [AttributeUsage(AttributeTargets.Property)]
    public class Requied : Attribute
    {

    }

    // thông tin không được phép trùng
    [AttributeUsage(AttributeTargets.Property)]
    public class NotAllowDuplicate : Attribute
    {

    }

    // Khóa chính
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {

    }

    // có trong model nhưng không có trong bảng db
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMap : Attribute
    {

    }

    // teen hiển thị tiếng việt
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayName : Attribute
    {
        public string Name { get; set; }
        public DisplayName(string name)
        {
            Name = name;
        }
    }
}
