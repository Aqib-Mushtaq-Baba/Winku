namespace Winku.ViewModels
{
    public class EditRegisterUserVM:RegisterUserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ExistingProfilePicture { get; set; }
    }
}
