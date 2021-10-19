using System.Collections.Generic;

namespace ConsoleApplication2
{
    public class SeaFightUserList
    {
        private List<User> users;

        private SeaFightUserList(List<User> newUsers)
        {
            users = new List<User>();
            foreach (var user in newUsers)
            {
                users.Add(user);
            }
        }
        
        
    }
}