//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeeNow.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class manager
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public manager()
        {
            this.announcement_log = new HashSet<announcement_log>();
            this.category_log = new HashSet<category_log>();
            this.difficulty_level_log = new HashSet<difficulty_level_log>();
            this.mall_log = new HashSet<mall_log>();
            this.post_violations = new HashSet<post_violations>();
            this.profile_log = new HashSet<profile_log>();
            this.quiz_violations = new HashSet<quiz_violations>();
            this.role_log = new HashSet<role_log>();
            this.user_violations = new HashSet<user_violations>();
        }

        [DisplayName("管理員帳號")]
        [Required(ErrorMessage = "欄位不可空白")]
        [StringLength(20, ErrorMessage = "欄位長度20字內")]
        public string manager_id { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage = "欄位不可空白")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼長度8~20字")]
        public string password { get; set; }

        [Display(Name = "確認密碼：")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請您再次輸入密碼！")]
        //與Password做比對，再次確認使用者輸入的密碼
        [Compare("password", ErrorMessage = "兩次輸入的密碼必須相符！")]
        public string ConfirmPassword { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<announcement_log> announcement_log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<category_log> category_log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<difficulty_level_log> difficulty_level_log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mall_log> mall_log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<post_violations> post_violations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<profile_log> profile_log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<quiz_violations> quiz_violations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<role_log> role_log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_violations> user_violations { get; set; }
    }
}