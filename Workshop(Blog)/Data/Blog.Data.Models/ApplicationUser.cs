// ReSharper disable VirtualMemberCallInConstructor

using System.ComponentModel.DataAnnotations;
using Blog.Data.Common.Constraints;
using Blog.Data.Common.Models;

namespace Blog.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Blog.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
        
        // User Info
        /// <summary>
        /// Displayed user name
        /// </summary>
        [MaxLength(ApplicationUserConstraints.UserNameMaxLength)]
        public override string UserName { get; set; } = null!;

        /// <summary>
        /// User Email
        /// </summary>
        [MaxLength(ApplicationUserConstraints.EmailMaxLength)]
        public override string Email { get; set; } = null!;
        
        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
