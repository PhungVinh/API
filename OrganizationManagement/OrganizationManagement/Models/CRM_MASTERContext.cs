using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OrganizationManagement.Constant;

namespace OrganizationManagement.Models
{
    public partial class CRM_MASTERContext : DbContext
    {
        public CRM_MASTERContext()
        {
        }

        public CRM_MASTERContext(DbContextOptions<CRM_MASTERContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAttributeAdvanced> TblAttributeAdvanced { get; set; }
        public virtual DbSet<TblAttributeConstraint> TblAttributeConstraint { get; set; }
        public virtual DbSet<TblAttributeOptions> TblAttributeOptions { get; set; }
        public virtual DbSet<TblAuthority> TblAuthority { get; set; }
        public virtual DbSet<TblAuthorityUser> TblAuthorityUser { get; set; }
        public virtual DbSet<TblCategory> TblCategory { get; set; }
        public virtual DbSet<TblCategoryGroup> TblCategoryGroup { get; set; }
        public virtual DbSet<TblConnectionConfig> TblConnectionConfig { get; set; }
        public virtual DbSet<TblEmailConfigs> TblEmailConfigs { get; set; }
        public virtual DbSet<TblEmailDownLoadedKey> TblEmailDownLoadedKey { get; set; }
        public virtual DbSet<TblEmailTemplate> TblEmailTemplate { get; set; }
        public virtual DbSet<TblLogUser> TblLogUser { get; set; }
        public virtual DbSet<TblMenu> TblMenu { get; set; }
        public virtual DbSet<TblOrganization> TblOrganization { get; set; }
        public virtual DbSet<TblOrganizationServicePack> TblOrganizationServicePack { get; set; }
        public virtual DbSet<TblOrganizationUser> TblOrganizationUser { get; set; }
        public virtual DbSet<TblRecieveEmail> TblRecieveEmail { get; set; }
        public virtual DbSet<TblRole> TblRole { get; set; }
        public virtual DbSet<TblRoleTest> TblRoleTest { get; set; }
        public virtual DbSet<TblServicePack> TblServicePack { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(OrganizationConstant.SQL_CONNECTION);
            }
        }


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

            modelBuilder.Entity<TblAttributeConstraint>(entity =>
            {
                entity.ToTable("tblAttributeConstraint");

                entity.Property(e => e.ContraintsType).HasMaxLength(100);

                entity.Property(e => e.ContraintsValue).HasMaxLength(100);

                entity.Property(e => e.ControlType).HasMaxLength(100);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LinkContraints)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

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

            modelBuilder.Entity<TblAuthority>(entity =>
            {
                entity.HasKey(e => e.AuthorityId);

                entity.Property(e => e.AuthorityDescription).HasMaxLength(500);

                entity.Property(e => e.AuthorityName).HasMaxLength(100);

                entity.Property(e => e.AuthorityType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateBy).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblAuthorityUser>(entity =>
            {
                entity.HasKey(e => e.AuthorityUserId);

                entity.ToTable("tblAuthorityUser");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblAuthorityUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblRoleUsers_tblUsers");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryCode).HasMaxLength(50);

                entity.Property(e => e.CategoryDescription).HasMaxLength(500);

                entity.Property(e => e.CategoryName).HasMaxLength(200);

                entity.Property(e => e.CategoryTypeCode).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

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

            modelBuilder.Entity<TblConnectionConfig>(entity =>
            {
                entity.ToTable("tblConnectionConfig");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConnectionKey).HasMaxLength(50);

                entity.Property(e => e.ConnectionValue).HasMaxLength(300);
            });

            modelBuilder.Entity<TblEmailConfigs>(entity =>
            {
                entity.ToTable("tblEmailConfigs");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MaximumPush).HasColumnName("Maximum_Push");

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PortGet)
                    .HasColumnName("Port_Get")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PortPush)
                    .HasColumnName("Port_Push")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ServerGet)
                    .HasColumnName("Server_Get")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ServerPush)
                    .HasColumnName("Server_Push")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Signature)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Ssl)
                    .HasColumnName("SSL")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEmailDownLoadedKey>(entity =>
            {
                entity.ToTable("tblEmailDownLoadedKey");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.From)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MessageId)
                    .HasColumnName("MessageID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.To).IsUnicode(false);
            });

            modelBuilder.Entity<TblEmailTemplate>(entity =>
            {
                entity.HasKey(e => e.TeamplateId);

                entity.ToTable("tblEmailTemplate");

                entity.Property(e => e.TeamplateId).HasColumnName("TeamplateID");

                entity.Property(e => e.AttachedFile).HasMaxLength(500);

                entity.Property(e => e.CreateBy).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EmailHeader).HasMaxLength(500);

                entity.Property(e => e.EmailName).HasMaxLength(500);

                entity.Property(e => e.UpdateBy).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblLogUser>(entity =>
            {
                entity.Property(e => e.ActionDate).HasColumnType("datetime");

                entity.Property(e => e.ActionName).HasMaxLength(250);

                entity.Property(e => e.ActionResult).HasMaxLength(250);

                entity.Property(e => e.ActionType).HasMaxLength(250);

                entity.Property(e => e.LoginBrowserName).HasMaxLength(250);

                entity.Property(e => e.LoginIpaddress)
                    .HasColumnName("LoginIPAddress")
                    .HasMaxLength(250);

                entity.Property(e => e.LoginOperatingSystemName).HasMaxLength(250);

                entity.Property(e => e.LoginTimeStart).HasColumnType("datetime");

                entity.Property(e => e.Module).HasMaxLength(250);

                entity.Property(e => e.OrganizationCode).HasMaxLength(250);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(250);
            });

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.ToTable("tblMenu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.MenuBadge).HasMaxLength(200);

                entity.Property(e => e.MenuCode).HasMaxLength(50);

                entity.Property(e => e.MenuIcon).HasMaxLength(100);

                entity.Property(e => e.MenuMainState)
                    .HasColumnName("MenuMain_State")
                    .HasMaxLength(50);

                entity.Property(e => e.MenuName).HasMaxLength(200);

                entity.Property(e => e.MenuShortLable)
                    .HasColumnName("MenuShort_Lable")
                    .HasMaxLength(50);

                entity.Property(e => e.MenuState).HasMaxLength(50);

                entity.Property(e => e.MenuType).HasMaxLength(50);

                entity.Property(e => e.ParentCode).HasMaxLength(50);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblOrganization>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);

                entity.ToTable("tblOrganization");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationAddress).HasMaxLength(200);

                entity.Property(e => e.OrganizationCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationFrom).HasColumnType("datetime");

                entity.Property(e => e.OrganizationHomePage).HasMaxLength(200);

                entity.Property(e => e.OrganizationName).HasMaxLength(200);

                entity.Property(e => e.OrganizationNote).HasMaxLength(200);

                entity.Property(e => e.OrganizationParentCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationPhone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationRemark).HasMaxLength(200);

                entity.Property(e => e.OrganizationSphereId).HasColumnName("OrganizationSphereID");

                entity.Property(e => e.OrganizationTaxCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationTo).HasColumnType("datetime");

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblOrganizationServicePack>(entity =>
            {
                entity.ToTable("tblOrganizationServicePack");
            });

            modelBuilder.Entity<TblOrganizationUser>(entity =>
            {
                entity.HasKey(e => e.OrganizationUserId);

                entity.ToTable("tblOrganizationUser");

                entity.Property(e => e.OrganizationUserId).HasColumnName("OrganizationUserID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.TblOrganizationUser)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_tblOrganizationUser_tblOrganization");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblOrganizationUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblOrganizationUser_tblUsers");
            });

            modelBuilder.Entity<TblRecieveEmail>(entity =>
            {
                entity.ToTable("tblRecieveEmail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttachFile)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Bcc)
                    .HasColumnName("BCC")
                    .IsUnicode(false);

                entity.Property(e => e.Cc)
                    .HasColumnName("CC")
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.From)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HtmlBody).IsUnicode(false);

                entity.Property(e => e.MessageId)
                    .HasColumnName("MessageID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenCode).IsUnicode(false);

                entity.Property(e => e.References).IsUnicode(false);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.StepStatus)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Subject).HasMaxLength(500);

                entity.Property(e => e.To)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tblRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsAdd).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsApprove).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleteAll).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsEdit).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsEditAll).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsEnable).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsEncypt).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsExport).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFirstExtend).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFouthExtend).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsImport).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPermission).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPrint).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSecondExtend).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsShow).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsShowAll).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsThirdExtend).HasDefaultValueSql("((0))");

                entity.Property(e => e.MenuCode).HasMaxLength(50);
            });

            modelBuilder.Entity<TblRoleTest>(entity =>
            {
                entity.ToTable("tblRoleTest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RoleDescription).HasMaxLength(500);

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.Property(e => e.RoleType).HasMaxLength(30);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblServicePack>(entity =>
            {
                entity.ToTable("tblServicePack");

                entity.Property(e => e.CodeServicePack).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.NameServicePack).HasMaxLength(250);

                entity.Property(e => e.UpdatedBy).HasMaxLength(250);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.ToTable("tblUsers");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.CategoryCodeDepartment).HasMaxLength(50);

                entity.Property(e => e.CategoryCodeRole).HasMaxLength(50);

                entity.Property(e => e.CodeReset).HasMaxLength(250);

                entity.Property(e => e.CreateBy).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateUpdatePassword).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.HistoryPassword).HasMaxLength(350);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LoginFail).HasDefaultValueSql("((0))");

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Position).HasMaxLength(50);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
