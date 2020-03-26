using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic;

namespace Generic.Firebase
{
    public class FirebaseService
    {
        private readonly FirebaseAuth auth;

        public FirebaseService()
        {
            auth = FirebaseAuth.DefaultInstance;
        }

        public bool IsAuthenticated()
        {
            return auth.CurrentUser != null;
        }

        public async Task<FirebaseUser> SignInAnonymouslyAsync()
        {
            return await auth.SignInAnonymouslyAsync();
        }

        public Task SignOut()
        {
            return auth.CurrentUser.DeleteAsync().ContinueWith(task =>
            {
                auth.SignOut();
            });
        }
    }
}