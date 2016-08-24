using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Storage;
using SecureChat.ServerAPI;
using SecureChatSharedComponents.DataModels;

namespace SecureChat
{
    public partial class FormCreateNewUser : Form
    {
        private const int MIN_PASSWORD_LENGTH = 8;
        public const int MIN_NICKNAME_LENGTH = 3;
        private const string VALID_NICKNAME_CHARS = "abcdefghijklmnopqerstuvwxyzABCDEFGHIJKLMNOPQERSTUVWXYZ0123456789_";
        private const string VALID_PASSWORD_CHARS = @"abcdefghijklmnopqerstuvwxyzABCDEFGHIJKLMNOPQERSTUVWXYZ0123456789_+?!|<>-/\:.=";
        private bool _passwordsConfirmed;
        private bool _usernameValidated;
        private RSAKeySetIdentity keySetIdentity = null;
        private readonly RSA_AsymetricEncryption rsaAsymetricEncryption;
        private SecureChatUser chatUserProfileData;

        public FormCreateNewUser()
        {
            rsaAsymetricEncryption = new RSA_AsymetricEncryption();
            InitializeComponent();
            
        }

        private async void InitializeServerCommunicationKey()
        {
            keySetIdentity = await GenerateRSA_Keys();
        }

        private async Task<RSAKeySetIdentity> GenerateRSA_Keys()
        {
            return await Task.Run(() => rsaAsymetricEncryption.GenerateRSAKeyPair(RSA_AsymetricEncryption.RSAKeySize.b4096));
        }

        private void FormCreateNewUser_Load(object sender, EventArgs e)
        {
            lblServerStatus.Text = "";
            lblKeyGenStatus.Text = "";
            InitializeServerCommunicationKey();
            SetInputBoxStatusImage(InputTypes.Nickname, InputStatus.Initial);
            SetInputBoxStatusImage(InputTypes.Password, InputStatus.Initial);
            SetInputBoxStatusImage(InputTypes.Password2, InputStatus.Initial);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text.Length < MIN_PASSWORD_LENGTH)
            {
                lblServerStatus.Text = string.Format("Password must be atleast {0} chars long", MIN_PASSWORD_LENGTH);
                e.Cancel = true;
            }

            else if (txtPassword.Text.Any(t => !VALID_PASSWORD_CHARS.Contains(t)))
            {
                lblServerStatus.Text = "Invalid password, non allowed characters used ";
                e.Cancel = true;
            }

            if(e.Cancel)
            {
                _passwordsConfirmed = false;
                SetInputBoxStatusImage(InputTypes.Password, InputStatus.Error);
            }
                
        }

        private void txtPasswordConfirm_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text.Length < MIN_PASSWORD_LENGTH)
            {
                _passwordsConfirmed = false;
                return;
            }

            if(txtPassword.Text != txtPasswordConfirm.Text)
            {
                lblServerStatus.Text = "Passwords did not match!";
                e.Cancel = true;
            }

            if (e.Cancel)
                SetInputBoxStatusImage(InputTypes.Password2, InputStatus.Error);
        }

        private void txtPassword_Validated(object sender, EventArgs e)
        {
            SetInputBoxStatusImage(InputTypes.Password, InputStatus.Ok);
            lblServerStatus.Text = "";    
        }

        private void txtPasswordConfirm_Validated(object sender, EventArgs e)
        {
            SetInputBoxStatusImage(InputTypes.Password2, InputStatus.Ok);
            lblServerStatus.Text = "";
            _passwordsConfirmed = true;
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if(txtUserName.Text.Length < MIN_NICKNAME_LENGTH)
            {
                lblServerStatus.Text = string.Format("Nickname must be atleast {0} chars long", MIN_NICKNAME_LENGTH);
                e.Cancel = true;
            }
            else if (txtUserName.Text.Any(t=> !VALID_NICKNAME_CHARS.Contains(t)))
            {
                lblServerStatus.Text = "Invalid nickname, non allowed characters used ";
                e.Cancel = true;
            }

            if(e.Cancel)
            {
                _usernameValidated = false;
                SetInputBoxStatusImage(InputTypes.Nickname, InputStatus.Error);
            }
        }

        private void txtUserName_Validated(object sender, EventArgs e)
        {
            SetInputBoxStatusImage(InputTypes.Nickname, InputStatus.Ok);
            lblServerStatus.Text = "";
            _usernameValidated = true;
        }

        private async void btnCreateUser_Click(object sender, EventArgs e)
        {
            txtPassword.Focus();
            txtPasswordConfirm.Focus();

            if(!_passwordsConfirmed)
            {
                MessageBox.Show(this, "Invalid password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_usernameValidated)
            {
                MessageBox.Show(this, "Invalid Username", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(keySetIdentity == null)
            {
                lblServerStatus.Text = "RSA keys for server comunication has not yet been completed, please try again in 5 seconds";
                return;
            }

            try
            {
                SecureChatCreateUserRequest userRequest = new SecureChatCreateUserRequest {Nickname = this.txtUserName.Text, RSA_PublicKey = this.keySetIdentity.RSA_PublicKey};
                chatUserProfileData = await ConnectionService.Instance.RegisterNewUser(userRequest, this.keySetIdentity);

                if(chatUserProfileData != null)
                {
                    saveFileDialog1.FileName = chatUserProfileData.NickName + ".scp";
                    saveFileDialog1.Filter = "Secure chat profiles|*.scp";
                    if(saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        StorageManager.SerializeObjectToFile(chatUserProfileData, saveFileDialog1.FileName, txtPassword.Text);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SetInputBoxStatusImage(InputTypes inputType, InputStatus inputStatus)
        {
            int imgIndex = (int)inputStatus;
            switch (inputType)
            {
                case InputTypes.Nickname:
                    picUserNameStatus.Image = statusImageList.Images[imgIndex];
                    break;
                case InputTypes.Password:
                    picPasswordStatus.Image = statusImageList.Images[imgIndex];
                    break;
                case InputTypes.Password2:
                    picPassword2Status.Image = statusImageList.Images[imgIndex];
                    break;
                default:
                    throw new ArgumentOutOfRangeException("inputType");
            }
        }

        private enum InputTypes
        {
            Nickname,
            Password,
            Password2
        }

        private enum InputStatus
        {
            Initial = 0,
            Ok = 1,
            Error = 2
        }

        private void txtPasswordConfirm_TextChanged(object sender, EventArgs e)
        {
            if (txtPasswordConfirm.Text.Length == 0)
            {
                txtPasswordConfirm.CausesValidation = false;
                _passwordsConfirmed = false;
                SetInputBoxStatusImage(InputTypes.Password2, InputStatus.Initial);
            }
            else if(!txtPasswordConfirm.CausesValidation && txtPasswordConfirm.Text.Length > 0)
            {
                txtPasswordConfirm.CausesValidation = true;
                txtPasswordConfirm.Focus();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if(txtPassword.Text.Length == 0)
            {
                txtPassword.CausesValidation = false;
                _passwordsConfirmed = false;
                SetInputBoxStatusImage(InputTypes.Password, InputStatus.Initial);
            }
            else if(!txtPassword.CausesValidation && txtPassword.Text.Length > 0)
            {
                txtPassword.CausesValidation = true;
                txtPassword.Focus();
            }

        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void txtPasswordConfirm_Enter(object sender, EventArgs e)
        {
            txtPasswordConfirm.SelectAll();
        }

        private void FormCreateNewUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
