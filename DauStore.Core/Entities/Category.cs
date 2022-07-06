using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Category : BaseEntity
    {
        private string categoryDescription;

        private List<string> categoryListDescription;

        public Category()
        {

        }

        [PrimaryKey]
        public Guid CategoryId { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string CategoryCode { get; set; }

        [Requied]
        public string CategoryName { get; set; }

        public int SeftCode { get; set; }

        public string ParentCode { get; set; }

        public bool IsExpandable { get; set; }

        public string CategoryDescription
        {
            get { return this.categoryDescription; }
            set
            {
                this.categoryDescription = value;
                if (this.categoryListDescription == null)
                    this.categoryListDescription = Newtonsoft.Json.JsonConvert.DeserializeObject<List<String>>(value);
            }
        }

        public List<string> CategoryListDescription
        {
            get { return this.categoryListDescription; }
            set { this.categoryListDescription = value; }
        }
    }
}
