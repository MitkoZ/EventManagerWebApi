using System.Web;

namespace EventManager.Helpers
{
    public class LoginUserSession
    {
        #region Properties
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public bool IsAuthenticated { get; private set; }
        #endregion

        #region Constructors
        private LoginUserSession()
        {
            IsAuthenticated = false;
        }
        #endregion

        #region Public properties
        public static LoginUserSession Current
        {
            get
            {
                LoginUserSession loginUserSession = (LoginUserSession)HttpContext.Current.Session["LoginUser"];
                if (loginUserSession == null)
                {
                    loginUserSession = new LoginUserSession();
                    HttpContext.Current.Session["LoginUser"] = loginUserSession;
                }
                return loginUserSession;
            }
        }
        #endregion

        #region public methods
        public void SetCurrentUser(int userId, string username)
        {
            this.IsAuthenticated = true;
            this.UserId = userId;
            this.Username = username;
        }

        public void Logout()
        {
            this.IsAuthenticated = false;
            this.UserId = 0;
            this.Username = string.Empty;
        }
        #endregion
    }
}