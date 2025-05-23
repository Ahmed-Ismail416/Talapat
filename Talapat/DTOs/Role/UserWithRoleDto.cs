﻿namespace Talapat.DTOs.Role
{
    public class UserWithRoleDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

}
