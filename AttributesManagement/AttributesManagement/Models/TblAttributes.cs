using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblAttributes
    {
        public int AttributesId { get; set; }
        public string AttributeCode { get; set; }
        public string AttributeType { get; set; }
        public string DataType { get; set; }
        public string AttributeLabel { get; set; }
        public string DefaultValue { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsTableShow { get; set; }
        public bool? IsCategory { get; set; }
        public string ModuleParent { get; set; }
        public bool? IsSort { get; set; }
        public bool? IsDuplicate { get; set; }
        public string DetailRefer { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpDateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public string IsReuse { get; set; }
        public string CategoryParentCode { get; set; }
        public string AttributeDescription { get; set; }
        public bool? Encyption { get; set; }
        public bool? EncyptWaiting { get; set; }
        public int? IndexTitleTable { get; set; }
        public string DefaultValueWithTextBox { get; set; }
        public bool? Disabled { get; set; }
        public bool? IsEnable { get; set; }
        public bool? IsLinkFieldInformation { get; set; }
        public string InputFieldValue { get; set; }
        public string IsDisplayFormat { get; set; }
        public string DisplayFormValue { get; set; }
        public int? SizeValue { get; set; }
        public string DescriptionTooltip { get; set; }
        public int? MaxValue { get; set; }
        public int? MinValue { get; set; }
        public string RuleInputValue { get; set; }
        public int? MaximumLength { get; set; }
        public int? MinimumLength { get; set; }
    }
}
