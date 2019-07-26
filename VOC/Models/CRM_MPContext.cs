using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VOC.Models
{
    public partial class CRM_MPContext : DbContext
    {
        public CRM_MPContext()
        {
        }

        public CRM_MPContext(DbContextOptions<CRM_MPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAttributeAdvanced> TblAttributeAdvanced { get; set; }
        public virtual DbSet<TblAttributeCondition> TblAttributeCondition { get; set; }
        public virtual DbSet<TblAttributeConstraint> TblAttributeConstraint { get; set; }
        public virtual DbSet<TblAttributeEvent> TblAttributeEvent { get; set; }
        public virtual DbSet<TblAttributeOperator> TblAttributeOperator { get; set; }
        public virtual DbSet<TblAttributeOptions> TblAttributeOptions { get; set; }
        public virtual DbSet<TblAttributes> TblAttributes { get; set; }
        public virtual DbSet<TblCategory> TblCategory { get; set; }
        public virtual DbSet<TblCategoryGroup> TblCategoryGroup { get; set; }
        public virtual DbSet<TblCimsattributeForm> TblCimsattributeForm { get; set; }
        public virtual DbSet<TblCimsattributeValue> TblCimsattributeValue { get; set; }
        public virtual DbSet<TblCimsform> TblCimsform { get; set; }
        public virtual DbSet<TblCimsFormHistory> TblCimsFormHistory { get; set; }
        public virtual DbSet<TblConnectionConfig> TblConnectionConfig { get; set; }
        public virtual DbSet<TblEncryption> TblEncryption { get; set; }
        public virtual DbSet<TblLog> TblLog { get; set; }
        public virtual DbSet<TblReferenceConstraint> TblReferenceConstraint { get; set; }
        public virtual DbSet<TblVocattributeForm> TblVocattributeForm { get; set; }
        public virtual DbSet<TblVocform> TblVocform { get; set; }
        public virtual DbSet<TblVocprocess> TblVocprocess { get; set; }
        public virtual DbSet<TblVocprocessAssignee> TblVocprocessAssignee { get; set; }
        public virtual DbSet<TblVocprocessConditions> TblVocprocessConditions { get; set; }
        public virtual DbSet<TblVocprocessSteps> TblVocprocessSteps { get; set; }
        public virtual DbSet<TblVocstepAttributes> TblVocstepAttributes { get; set; }
        public virtual DbSet<TblVocstepAttributesValue> TblVocstepAttributesValue { get; set; }
        public virtual DbSet<TblVocsteps> TblVocsteps { get; set; }
        public virtual DbSet<TblVocworkflows> TblVocworkflows { get; set; }
        public virtual DbSet<TblVocworkflowSteps> TblVocworkflowSteps { get; set; }

        // Unable to generate entity type for table 'dbo.tblCustomer'. Please see the warning messages.

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=192.168.50.149;initial catalog=CRM_Standard;user id=sa;password=123@123a");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAttributeAdvanced>(entity =>
            {
                entity.HasKey(e => e.AdvancedId);

                entity.ToTable("tblAttributeAdvanced");

                entity.Property(e => e.AdvancedDataType).HasMaxLength(50);

                entity.Property(e => e.AdvancedDescription).HasMaxLength(100);

                entity.Property(e => e.AdvancedName).HasMaxLength(100);

                entity.Property(e => e.AdvancedObject).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblAttributeCondition>(entity =>
            {
                entity.ToTable("tblAttributeCondition");

                entity.Property(e => e.AttributeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionValue).HasMaxLength(300);

                entity.Property(e => e.FunctionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblAttributeConstraint>(entity =>
            {
                entity.ToTable("tblAttributeConstraint");

                entity.Property(e => e.ContraintsType).HasMaxLength(250);

                entity.Property(e => e.ContraintsValue).HasMaxLength(250);

                entity.Property(e => e.ControlType).HasMaxLength(250);

                entity.Property(e => e.CreateBy).HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LinkContraints).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.UpdateBy).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblAttributeEvent>(entity =>
            {
                entity.ToTable("tblAttributeEvent");

                entity.Property(e => e.AffectedDetail).HasMaxLength(500);

                entity.Property(e => e.AttributeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventValue).HasMaxLength(500);
            });

            modelBuilder.Entity<TblAttributeOperator>(entity =>
            {
                entity.ToTable("tblAttributeOperator");

                entity.Property(e => e.AttributeCode).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModuleCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OperatorCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OperatorDetail).HasMaxLength(300);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblAttributeOptions>(entity =>
            {
                entity.HasKey(e => e.AttributeOptionsId);

                entity.ToTable("tblAttributeOptions");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OptionLabel).HasMaxLength(100);

                entity.Property(e => e.OptionValue).HasMaxLength(50);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblAttributes>(entity =>
            {
                entity.HasKey(e => e.AttributesId);

                entity.ToTable("tblAttributes");

                entity.Property(e => e.AttributeCode).HasMaxLength(50);

                entity.Property(e => e.AttributeDescription).HasMaxLength(500);

                entity.Property(e => e.AttributeLabel).HasMaxLength(100);

                entity.Property(e => e.AttributeType).HasMaxLength(50);

                entity.Property(e => e.CategoryParentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DataType).HasMaxLength(50);

                entity.Property(e => e.DefaultValue).HasMaxLength(200);

                entity.Property(e => e.DefaultValueWithTextBox).HasMaxLength(200);

                entity.Property(e => e.DetailRefer).HasMaxLength(500);

                entity.Property(e => e.ModuleParent).HasMaxLength(100);

                entity.Property(e => e.UpDateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryCode).HasMaxLength(200);

                entity.Property(e => e.CategoryDescription).HasMaxLength(500);

                entity.Property(e => e.CategoryName).HasMaxLength(200);

                entity.Property(e => e.CategoryTypeCode).HasMaxLength(200);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExtContent).HasMaxLength(500);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCategoryGroup>(entity =>
            {
                entity.ToTable("tblCategoryGroup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryCode).HasMaxLength(50);

                entity.Property(e => e.CategoryDescription).HasMaxLength(500);

                entity.Property(e => e.CategoryGroupName).HasMaxLength(200);

                entity.Property(e => e.CategoryTypeCode).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCimsattributeForm>(entity =>
            {
                entity.HasKey(e => e.AttributeFormId);

                entity.ToTable("tblCIMSAttributeForm");

                entity.Property(e => e.AttributeCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AttributeType).HasMaxLength(50);

                entity.Property(e => e.ChildCode).HasMaxLength(50);

                entity.Property(e => e.DefaultValue).HasMaxLength(50);

                entity.Property(e => e.RowTitle).HasMaxLength(150);
            });

            modelBuilder.Entity<TblCimsattributeValue>(entity =>
            {
                entity.HasKey(e => e.AttributesValueId);

                entity.ToTable("tblCIMSAttributeValue");

                entity.Property(e => e.AttributeCode).HasMaxLength(100);

                entity.Property(e => e.AttributeValue).HasColumnType("ntext");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Module)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RecordId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCimsform>(entity =>
            {
                entity.HasKey(e => e.FormId);

                entity.ToTable("tblCIMSForm");

                entity.Property(e => e.ChildCode).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FormDescription).HasMaxLength(100);

                entity.Property(e => e.FormName).HasMaxLength(50);

                entity.Property(e => e.FormType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MenuCode).HasMaxLength(50);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCimsFormHistory>(entity =>
            {
                entity.ToTable("tblCimsFormHistory");

                entity.Property(e => e.ChildCode).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblConnectionConfig>(entity =>
            {
                entity.ToTable("tblConnectionConfig");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConnectionKey).HasMaxLength(50);

                entity.Property(e => e.ConnectionValue).HasMaxLength(255);
            });

            modelBuilder.Entity<TblEncryption>(entity =>
            {
                entity.HasKey(e => e.EncryptionId);

                entity.ToTable("tblEncryption");

                entity.Property(e => e.AttributeCode).HasMaxLength(50);

                entity.Property(e => e.AttributeLabel).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.Field).HasMaxLength(50);

                entity.Property(e => e.ModuleName).HasMaxLength(50);

                entity.Property(e => e.OrgCode).HasMaxLength(50);

                entity.Property(e => e.ParentCode).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<TblLog>(entity =>
            {
                entity.ToTable("tblLog");

                entity.Property(e => e.ActionCode).HasMaxLength(50);

                entity.Property(e => e.ActionName).HasMaxLength(100);

                entity.Property(e => e.AttributeCode).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModuleParentCode).HasMaxLength(50);

                entity.Property(e => e.ObjectId).HasMaxLength(255);

                entity.Property(e => e.Value).HasMaxLength(100);
            });

            modelBuilder.Entity<TblReferenceConstraint>(entity =>
            {
                entity.ToTable("tblReferenceConstraint");

                entity.Property(e => e.AttributeCode).HasMaxLength(100);

                entity.Property(e => e.MenuCode).HasMaxLength(50);
            });

            modelBuilder.Entity<TblVocattributeForm>(entity =>
            {
                entity.HasKey(e => e.AttributeFormId);

                entity.ToTable("tblVOCAttributeForm");
            });

            modelBuilder.Entity<TblVocform>(entity =>
            {
                entity.HasKey(e => e.FormId);

                entity.ToTable("tblVOCForm");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FormDescription).HasMaxLength(500);

                entity.Property(e => e.FormName).HasMaxLength(100);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblVocprocess>(entity =>
            {
                entity.ToTable("tblVOCProcess");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.CreateBy).HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.UpdateBy).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.VOCProcessCode)
                    .HasColumnName("VOCProcessCode")
                    .HasMaxLength(250);

                entity.Property(e => e.VOCProcessName)
                    .HasColumnName("VOCProcessName")
                    .HasMaxLength(250);

                entity.Property(e => e.VOCProcessType)
                    .HasColumnName("VOCProcessType")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TblVocprocessAssignee>(entity =>
            {
                entity.ToTable("tblVOCProcessAssignee");

                entity.Property(e => e.StepCode).HasMaxLength(250);

                entity.Property(e => e.VocprocessCode)
                    .HasColumnName("VOCProcessCode")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TblVocprocessConditions>(entity =>
            {
                entity.ToTable("tblVOCProcessConditions");

                entity.Property(e => e.Condition).HasMaxLength(250);

                entity.Property(e => e.ConditionType).HasMaxLength(100);

                entity.Property(e => e.Field).HasMaxLength(50);

                entity.Property(e => e.NextCompare).HasMaxLength(50);

                entity.Property(e => e.StepCode).HasMaxLength(250);

                entity.Property(e => e.StepField).HasMaxLength(250);

                entity.Property(e => e.Value).HasMaxLength(250);

                entity.Property(e => e.VocprocessCode)
                    .HasColumnName("VOCProcessCode")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TblVocprocessSteps>(entity =>
            {
                entity.ToTable("tblVOCProcessSteps");

                entity.Property(e => e.CreateBy).HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FinishDate).HasColumnType("datetime");

                entity.Property(e => e.ParentCode).HasMaxLength(250);

                entity.Property(e => e.StepCode).HasMaxLength(250);

                entity.Property(e => e.StepName).HasMaxLength(250);

                entity.Property(e => e.UpdateBy).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.VocprocessCode)
                    .HasColumnName("VOCProcessCode")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TblVocstepAttributes>(entity =>
            {
                entity.HasKey(e => e.StepAttributesId);

                entity.ToTable("tblVOCStepAttributes");
            });

            modelBuilder.Entity<TblVocstepAttributesValue>(entity =>
            {
                entity.HasKey(e => e.StepAttributesValueId);

                entity.ToTable("tblVOCStepAttributesValue");

                entity.Property(e => e.AttributeValue).HasColumnType("text");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Step)
                    .WithMany(p => p.TblVocstepAttributesValue)
                    .HasForeignKey(d => d.StepId)
                    .HasConstraintName("FK_tblVOCStepAttributesValue_tblVOCSteps");
            });

            modelBuilder.Entity<TblVocsteps>(entity =>
            {
                entity.HasKey(e => e.StepId);

                entity.ToTable("tblVOCSteps");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.StepName).HasMaxLength(200);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblVocworkflows>(entity =>
            {
                entity.HasKey(e => e.WorkflowId);

                entity.ToTable("tblVOCWorkflows");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.WorkflowName).HasMaxLength(200);
            });

            modelBuilder.Entity<TblVocworkflowSteps>(entity =>
            {
                entity.HasKey(e => e.WorkflowStepsId);

                entity.ToTable("tblVOCWorkflowSteps");
            });
        }
    }
}
