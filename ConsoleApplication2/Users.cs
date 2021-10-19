namespace ConsoleApplication2
{
    public struct User
    {
        public string login;
        public string pass;
        public User(string login, string pass)
        {
            this.login = login;
            this.pass = pass;
        }
    }
}