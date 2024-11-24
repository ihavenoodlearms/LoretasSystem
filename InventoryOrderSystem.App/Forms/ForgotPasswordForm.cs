using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using InventoryOrderSystem.Services;
using System.Text.RegularExpressions;

namespace InventoryOrderSystem
{
    public partial class ForgotPasswordForm : Form
    {
        private DatabaseManager dbManager;
        private TextBox txtUsername;
        private TextBox txtEmail;
        private ComboBox cboSecurityQuestion1;
        private TextBox txtSecurityAnswer1;
        private ComboBox cboSecurityQuestion2;
        private TextBox txtSecurityAnswer2;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnResetPassword;
        private Label lblMessage;
        private Panel panel1;
        private Label lblTitle;
        private Label lblPasswordStrength;
        private ProgressBar passwordStrengthBar;
        private System.Windows.Forms.Timer lockoutTimer;
        private int verificationStep = 1;

        private readonly string[] SecurityQuestions = new string[]
        {
            "What was your first pet's name?",
            "In which city were you born?",
            "What was your mother's maiden name?",
            "What was the name of your first school?",
            "What is your favorite book?",
            "What was your childhood nickname?"
        };

        public ForgotPasswordForm()
        {
            dbManager = new DatabaseManager();
            InitializeComponents();
            SetupPasswordStrengthMeter();
            InitializeLockoutTimer();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(450, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;

            // Panel for title
            panel1 = new Panel
            {
                BackColor = Color.FromArgb(59, 48, 48),
                Dock = DockStyle.Top,
                Height = 80
            };

            lblTitle = new Label
            {
                Text = "Reset Password",
                ForeColor = Color.FromArgb(255, 240, 209),
                Font = new Font("Britannic Bold", 20),
                Location = new Point(125, 25),
                AutoSize = true
            };
            panel1.Controls.Add(lblTitle);

            // Step 1: Username and Email verification
            int currentY = 100;

            // Username input
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtUsername = new TextBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                Font = new Font("Arial", 10)
            };

            currentY += 70;

            // Email input
            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtEmail = new TextBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                Font = new Font("Arial", 10)
            };

            currentY += 70;

            // Security Questions
            Label lblSecurityQ1 = new Label
            {
                Text = "Security Question 1:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            cboSecurityQuestion1 = new ComboBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };
            cboSecurityQuestion1.Items.AddRange(SecurityQuestions);

            currentY += 70;

            Label lblSecurityA1 = new Label
            {
                Text = "Answer 1:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtSecurityAnswer1 = new TextBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                Font = new Font("Arial", 10)
            };

            currentY += 70;

            // Security Question 2
            Label lblSecurityQ2 = new Label
            {
                Text = "Security Question 2:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            cboSecurityQuestion2 = new ComboBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };
            cboSecurityQuestion2.Items.AddRange(SecurityQuestions);

            currentY += 70;

            Label lblSecurityA2 = new Label
            {
                Text = "Answer 2:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtSecurityAnswer2 = new TextBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                Font = new Font("Arial", 10)
            };

            currentY += 70;

            // Password section (initially hidden)
            lblPasswordStrength = new Label
            {
                Text = "Password Strength:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            passwordStrengthBar = new ProgressBar
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 20),
                Minimum = 0,
                Maximum = 100
            };

            currentY += 70;

            Label lblNewPassword = new Label
            {
                Text = "New Password:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtNewPassword = new TextBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                UseSystemPasswordChar = true,
                Font = new Font("Arial", 10)
            };

            currentY += 70;

            Label lblConfirmPassword = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(50, currentY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(50, currentY + 25),
                Size = new Size(350, 25),
                UseSystemPasswordChar = true,
                Font = new Font("Arial", 10)
            };

            currentY += 70;

            // Reset Button
            btnResetPassword = new Button
            {
                Text = "Next",
                Location = new Point(50, currentY),
                Size = new Size(350, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(82, 110, 72),
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 9.75f)
            };
            btnResetPassword.Click += btnResetPassword_Click;

            currentY += 50;

            // Message Label
            lblMessage = new Label
            {
                Location = new Point(50, currentY),
                Size = new Size(350, 40),
                Font = new Font("Arial", 9),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Back Button
            Button btnBack = new Button
            {
                Text = "Back to Login",
                Location = new Point(50, currentY + 50),
                Size = new Size(350, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(59, 48, 48),
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 9.75f)
            };
            btnBack.Click += (s, e) => this.Close();

            // Add all controls
            this.Controls.AddRange(new Control[] {
                panel1,
                lblUsername, txtUsername,
                lblEmail, txtEmail,
                lblSecurityQ1, cboSecurityQuestion1,
                lblSecurityA1, txtSecurityAnswer1,
                lblSecurityQ2, cboSecurityQuestion2,
                lblSecurityA2, txtSecurityAnswer2,
                lblPasswordStrength, passwordStrengthBar,
                lblNewPassword, txtNewPassword,
                lblConfirmPassword, txtConfirmPassword,
                btnResetPassword, lblMessage,
                btnBack
            });

            // Initially hide password controls

        }

        private void InitializeLockoutTimer()
        {
            lockoutTimer = new System.Windows.Forms.Timer();
            lockoutTimer.Interval = 1000; // 1 second
            lockoutTimer.Tick += LockoutTimer_Tick;
        }

        private void LockoutTimer_Tick(object sender, EventArgs e)
        {
            int remainingSeconds = int.Parse(lblMessage.Tag.ToString());
            remainingSeconds--;

            if (remainingSeconds <= 0)
            {
                lockoutTimer.Stop();
                btnResetPassword.Enabled = true;
                lblMessage.Text = "";
            }
            else
            {
                lblMessage.Text = $"Please wait {remainingSeconds} seconds before trying again.";
                lblMessage.Tag = remainingSeconds.ToString();
            }
        }

        private void HidePasswordControls()
        {
            lblPasswordStrength.Visible = false;
            passwordStrengthBar.Visible = false;
            txtNewPassword.Visible = false;
            txtConfirmPassword.Visible = false;
            lblPasswordStrength.Visible = false;
            passwordStrengthBar.Visible = false;
            btnResetPassword.Text = "Next";
        }

        private void ShowPasswordControls()
        {
            // Hide the security question controls
            cboSecurityQuestion1.Visible = false;
            txtSecurityAnswer1.Visible = false;
            cboSecurityQuestion2.Visible = false;
            txtSecurityAnswer2.Visible = false;

            // Show password related controls
            lblPasswordStrength.Visible = true;
            passwordStrengthBar.Visible = true;
            txtNewPassword.Visible = true;
            txtConfirmPassword.Visible = true;

            // Update labels visibility
            this.Controls.Find("Security Question 1:", false).FirstOrDefault()?.Hide();
            this.Controls.Find("Answer 1:", false).FirstOrDefault()?.Hide();
            this.Controls.Find("Security Question 2:", false).FirstOrDefault()?.Hide();
            this.Controls.Find("Answer 2:", false).FirstOrDefault()?.Hide();
            this.Controls.Find("New Password:", false).FirstOrDefault()?.Show();
            this.Controls.Find("Confirm Password:", false).FirstOrDefault()?.Show();

            btnResetPassword.Text = "Reset Password";

            // Adjust the form layout
            AdjustControlPositions();
        }

        private void AdjustControlPositions()
        {
            int currentY = 100;

            // Keep username and email at the top
            var usernameLabel = this.Controls.Find("Username:", false).FirstOrDefault();
            if (usernameLabel != null)
            {
                usernameLabel.Location = new Point(50, currentY);
                txtUsername.Location = new Point(50, currentY + 25);
            }

            currentY += 70;

            var emailLabel = this.Controls.Find("Email:", false).FirstOrDefault();
            if (emailLabel != null)
            {
                emailLabel.Location = new Point(50, currentY);
                txtEmail.Location = new Point(50, currentY + 25);
            }

            currentY += 70;

            // Password strength indicator
            lblPasswordStrength.Location = new Point(50, currentY);
            passwordStrengthBar.Location = new Point(50, currentY + 25);

            currentY += 70;

            // New password
            var newPasswordLabel = this.Controls.Find("New Password:", false).FirstOrDefault();
            if (newPasswordLabel != null)
            {
                newPasswordLabel.Location = new Point(50, currentY);
                txtNewPassword.Location = new Point(50, currentY + 25);
            }

            currentY += 70;

            // Confirm password
            var confirmPasswordLabel = this.Controls.Find("Confirm Password:", false).FirstOrDefault();
            if (confirmPasswordLabel != null)
            {
                confirmPasswordLabel.Location = new Point(50, currentY);
                txtConfirmPassword.Location = new Point(50, currentY + 25);
            }

            currentY += 70;

            // Reset button
            btnResetPassword.Location = new Point(50, currentY);

            currentY += 50;

            // Message label
            lblMessage.Location = new Point(50, currentY);
        }

        private void SetupPasswordStrengthMeter()
        {
            txtNewPassword.TextChanged += (s, e) =>
            {
                int strength = CalculatePasswordStrength(txtNewPassword.Text);
                passwordStrengthBar.Value = strength;

                if (strength < 40)
                {
                    passwordStrengthBar.ForeColor = Color.Red;
                    lblPasswordStrength.ForeColor = Color.Red;
                    lblPasswordStrength.Text = "Password Strength: Weak";
                }
                else if (strength < 75)
                {
                    passwordStrengthBar.ForeColor = Color.Yellow;
                    lblPasswordStrength.ForeColor = Color.Orange;
                    lblPasswordStrength.Text = "Password Strength: Medium";
                }
                else
                {
                    passwordStrengthBar.ForeColor = Color.Green;
                    lblPasswordStrength.ForeColor = Color.Green;
                    lblPasswordStrength.Text = "Password Strength: Strong";
                }
            };
        }

        private int CalculatePasswordStrength(string password)
        {
            int score = 0;

            if (string.IsNullOrEmpty(password))
                return score;

            // Length
            if (password.Length >= 8) score += 20;
            if (password.Length >= 12) score += 10;

            // Complexity
            if (password.Any(char.IsUpper)) score += 15;
            if (password.Any(char.IsLower)) score += 15;
            if (password.Any(char.IsDigit)) score += 15;
            if (password.Any(ch => !char.IsLetterOrDigit(ch))) score += 15;

            // Variety
            var uniqueChars = password.Distinct().Count();
            score += Math.Min((uniqueChars * 2), 10);

            return Math.Min(score, 100);
        }

        private async void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (verificationStep == 1)
                {
                    if (await VerifyUserAndSecurityQuestions())
                    {
                        verificationStep = 2;
                        ShowPasswordControls();
                        lblMessage.Text = "Please enter your new password.";
                        lblMessage.ForeColor = Color.Green;
                    }
                }
                else
                {
                    await ResetPassword();
                }
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "An error occurred. Please try again.";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update the verification method to use explicit null checking
        private async Task<bool> VerifyUserAndSecurityQuestions()
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string securityAnswer1 = txtSecurityAnswer1.Text.Trim();
            string securityAnswer2 = txtSecurityAnswer2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(securityAnswer1) || string.IsNullOrEmpty(securityAnswer2))
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Please fill in all fields.";
                return false;
            }

            try
            {
                if (!await Task.Run(() => dbManager.VerifyUserSecurityInfo(username, email, securityAnswer1, securityAnswer2)))
                {
                    await Task.Run(() => dbManager.IncrementFailedResetAttempts(username));

                    int failedAttempts = await Task.Run(() => dbManager.GetFailedResetAttempts(username));
                    if (failedAttempts >= 3)
                    {
                        int lockoutDuration = 300; // 5 minutes
                        btnResetPassword.Enabled = false;
                        lblMessage.Tag = lockoutDuration.ToString();
                        lblMessage.Text = $"Please wait {lockoutDuration} seconds before trying again.";
                        lockoutTimer.Start();
                        return false;
                    }

                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Invalid username, email, or security answers.";
                    return false;
                }

                // Get last password reset time and check the 24-hour restriction
                DateTime? lastReset = await Task.Run(() => dbManager.GetLastPasswordReset(username));

                // Explicit null check for C# 7.3 compatibility
                if (lastReset.HasValue)
                {
                    DateTime lastResetValue = lastReset.Value;
                    TimeSpan timeSinceReset = DateTime.Now - lastResetValue;
                    if (timeSinceReset.TotalHours < 24)
                    {
                        lblMessage.ForeColor = Color.Red;
                        lblMessage.Text = "Password was reset recently. Please wait 24 hours between resets.";
                        return false;
                    }
                }

                await Task.Run(() => dbManager.ResetFailedAttempts(username));
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "An error occurred during verification.";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task ResetPassword()
        {
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string username = txtUsername.Text.Trim();

            // Password validation
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Please enter and confirm your new password.";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            // Check password strength
            int passwordStrength = CalculatePasswordStrength(newPassword);
            if (passwordStrength < 75) // Requires strong password
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Password is too weak. Please ensure it contains:\n" +
                                "- At least 8 characters\n" +
                                "- Uppercase and lowercase letters\n" +
                                "- Numbers and special characters";
                return;
            }

            try
            {
                // Get user and update password
                var user = await Task.Run(() => dbManager.GetUserByUsername(username));
                if (user != null)
                {
                    await Task.Run(() =>
                    {
                        dbManager.UpdateUserPassword(user.UserId, newPassword);
                        dbManager.UpdateLastPasswordReset(username);
                    });

                    lblMessage.ForeColor = Color.Green;
                    lblMessage.Text = "Password reset successful! Returning to login...";

                    // Disable controls after successful reset
                    btnResetPassword.Enabled = false;
                    txtNewPassword.Enabled = false;
                    txtConfirmPassword.Enabled = false;

                    // Close the form after 2 seconds
                    Timer timer = new Timer();
                    timer.Interval = 2000;
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        this.Close();
                    };
                    timer.Start();
                }
                else
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "User not found. Please try again.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "An error occurred while resetting the password.";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Draw a border around the form
            using (Pen pen = new Pen(Color.FromArgb(59, 48, 48), 2))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }
    }
}