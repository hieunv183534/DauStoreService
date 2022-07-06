using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Item : BaseEntity
    {
        private string description;

        private List<string> listDescription;

        public Item()
        {

        }

        [PrimaryKey]
        public Guid ItemId { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string ItemCode { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string ItemName { get; set; }

        public string Description 
        {
            get { return this.description; }
            set
            {
                this.description = value;
                if (this.listDescription == null)
                    this.listDescription = Newtonsoft.Json.JsonConvert.DeserializeObject<List<String>>(value);
            }
        }

        public List<string> ListDescription
        {
            get { return this.listDescription; }
            set { this.listDescription = value; }
        }

        [Requied]
        public double RealPrice { get; set; }

        [Requied]
        public float SaleRate { get; set; }

        [Requied]
        public string Medias { get; set; }

        public string Tag { get; set; }

        [Requied]
        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public int InStock { get; set; }
    }
}
