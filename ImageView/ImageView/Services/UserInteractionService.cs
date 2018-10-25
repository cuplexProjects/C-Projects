using System;
using System.Collections.Generic;
using System.Linq;
using ImageViewer.Models.UserInteraction;
using JetBrains.Annotations;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class UserInteractionService : ServiceBase
    {
        private readonly Queue<UserInteractionQuestion> _questionQueue;
        private readonly Queue<UserInteractionInformation> _informationQueue;
        private FormMain _formMain;
        public bool IsInitialized { get; private set; }
        public event EventHandler<UserQuestionEventArgs> UserQuestionRecieved;
        public event EventHandler<UserInformationEventArgs> UserInformationRecieved;

        public UserInteractionService()
        {
            _questionQueue = new Queue<UserInteractionQuestion>();
            _informationQueue = new Queue<UserInteractionInformation>();
        }

        public void Initialize(FormMain formMain)
        {
            _formMain = formMain ?? throw new ArgumentException("Form main can not be null");
            _formMain.Shown += _formMain_Shown;
        }

        private void _formMain_Shown(object sender, EventArgs e)
        {
            IsInitialized = true;

            if (UserInformationRecieved != null && _informationQueue.Any())
            {
                while (_informationQueue.Any())
                {
                    var infoItem = _informationQueue.Dequeue();
                    UserInformationRecieved.Invoke(this, new UserInformationEventArgs(infoItem));
                }
            }

            if (UserQuestionRecieved != null && _questionQueue.Any())
            {
                while (_questionQueue.Any())
                {
                    var questionItem = _questionQueue.Dequeue();
                    UserQuestionRecieved.Invoke(this, new UserQuestionEventArgs(questionItem));
                }
            }
        }

        public bool AnyUserQuestionsEnqued => _questionQueue.Count > 0;
        public bool AnyUserInformationEnqued => _informationQueue.Count > 0;

        public Queue<UserInteractionQuestion>.Enumerator GetInteractionQuestions()
        {
            return _questionQueue.GetEnumerator();
        }

        public Queue<UserInteractionInformation>.Enumerator GetInteractionInformation()
        {
            return _informationQueue.GetEnumerator();
        }

        public void RequestUserAccept(UserInteractionQuestion userAccept)
        {
            if (UserQuestionRecieved == null)
            {
                _questionQueue.Enqueue(userAccept);
                return;
            }

            UserQuestionRecieved.Invoke(this, new UserQuestionEventArgs(userAccept));
        }

        public void InformUser(UserInteractionInformation userInform)
        {
            if (UserInformationRecieved == null)
            {
                _informationQueue.Enqueue(userInform);
                return;
            }


            UserInformationRecieved?.Invoke(this, new UserInformationEventArgs(userInform));
        }
    }
}
