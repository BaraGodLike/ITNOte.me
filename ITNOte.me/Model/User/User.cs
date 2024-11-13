using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;

namespace ITNOte.me.Model.User
{
    public class User
    {
        public string Name { get; init; }
        public byte[]? Password { get; set; }
        public Folder GeneralFolder { get; set; }
        public int General_Folder_Id { get; set; }

        public User()
        {
            
        }
        
        public User(string name, byte[]? password, Folder genfolder)
        {
            Name = name;
            Password = password;
            GeneralFolder = genfolder; 
        }
        
        public User(string name, byte[]? password)
        {
            Name = name;
            Password = password;
            GeneralFolder = new Folder(name); 
        }

        public override string ToString() => Name;
    }
}