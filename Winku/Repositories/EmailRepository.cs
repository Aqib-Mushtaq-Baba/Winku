using Winku.DatabaseFolder;
using Winku.Interfaces;

namespace Winku.Repositories
{
    public class EmailRepository : IEmailInterface
    {
        private readonly AppDbContext context;

        public EmailRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Email AddEmail(Email email)
        {
            context.Emails.Add(email);
            context.SaveChanges();
            return email;
        }
    }
}
