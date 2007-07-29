using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.BusinessLogic;
using Umbraco.Cms.BusinessLogic.language;
using Umbraco.Cms.BusinessLogic.property;
using Umbraco.Cms.BusinessLogic.task;
using Umbraco.Cms.BusinessLogic.web;

namespace Umbraco.Cms.BusinessLogic.translation
{
    public class Translation
    {
        public static void MakeNew(CMSNode Node, User User, User Translator, Language Language, string Comment,
                                   bool IncludeSubpages, bool SendEmail)
        {
            // Create pending task
            Task t = new Task();
            t.Comment = Comment;
            t.Node = Node;
            t.ParentUser = User;
            t.User = Translator;
            t.Type = new TaskType("toTranslate");
            t.Save();

            // Add log entry
            Log.Add(LogTypes.SendToTranslate, User, Node.Id,
                    "Translator: " + Translator.Name + ", Language: " + Language.FriendlyName);

            // send it
            if (SendEmail)
            {
                // Send mail
                string[] subjectVars = {HttpContext.Current.Request.ServerVariables["SERVER_NAME"], Node.Text};
                string[] bodyVars = {
                                        Translator.Name, Node.Text, User.Name,
                                        HttpContext.Current.Request.ServerVariables["SERVER_NAME"], Node.Id.ToString(),
                                        Language.FriendlyName
                                    };

                if (User.Email != "" && User.Email.Contains("@") && Translator.Email != "" &&
                    Translator.Email.Contains("@"))
                {
                    // create the mail message 
                    MailMessage mail = new MailMessage(User.Email, Translator.Email);

                    // populate the message
                    mail.Subject = ui.Text("translation", "mailSubject", subjectVars, Translator);
                    mail.IsBodyHtml = false;
                    mail.Body = ui.Text("translation", "mailBody", bodyVars, Translator);
                    try
                    {
                        SmtpClient sender = new SmtpClient(GlobalSettings.SmtpServer);
                        sender.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        Log.Add(LogTypes.Error, User, Node.Id,
                                string.Format("Error sending translation e-mail:{0}", ex.ToString()));
                    }
                }
                else
                    Log.Add(LogTypes.Error, User, Node.Id,
                            "Could not send translation e-mail because either user or translator lacks e-mail in settings");
            }

            if (IncludeSubpages)
            {
                foreach (CMSNode n in Node.Children)
                    MakeNew(n, User, Translator, Language, Comment, true, false);
            }
        }

        public static int CountWords(int DocumentId)
        {
            Document d = new Document(DocumentId);

            int words = CountWordsInString(d.Text);
            foreach (Property p in d.getProperties)
            {
                if (p.Value.GetType() == "".GetType())
                {
                    if (p.Value.ToString().Trim() != "")
                        words += CountWordsInString(p.Value.ToString());
                }
            }

            return words;
        }

        private static int CountWordsInString(string Text)
        {
            string pattern = @"<(.|\n)*?>";
            string tmpStr = Regex.Replace(Text, pattern, string.Empty);

            tmpStr = tmpStr.Replace("\t", " ").Trim();
            tmpStr = tmpStr.Replace("\n", " ");
            tmpStr = tmpStr.Replace("\r", " ");

            while (tmpStr.IndexOf("  ") != -1)
                tmpStr = tmpStr.Replace("  ", " ");

            return tmpStr.Split(' ').Length;
        }
    }
}