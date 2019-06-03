using System;

namespace UnityLocalize.Exceptions
{
    public class NotInitializedException : Exception
    {
        public override string Message { get; }

        public NotInitializedException(Type type, string objectName = null)
        {
            string message;

            if (type.IsAbstract && type.IsSealed)
            {
                message = $"Static class: '{type.Name}' is not initialized.";
            }
            else
            {
                if (string.IsNullOrEmpty(objectName))
                {
                    message = $"Object of type: '{type.Name}' is not initialized.";
                }
                else
                {
                    message = $"Object '{objectName}' of type: '{type.Name}' is not initialized.";
                }
            }

            this.Message = message;
        }
    }
}
