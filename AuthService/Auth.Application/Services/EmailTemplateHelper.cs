namespace Auth.Application.Services
{
    public static class EmailTemplateHelper
    {

        public static string GetConfirmationEmail(string name, string code)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; background: #f5f5f5; margin: 0; padding: 20px; }}
                        .container {{ max-width: 600px; margin: 0 auto; background: white; border-radius: 8px; padding: 30px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                        .header {{ text-align: center; margin-bottom: 30px; }}
                        .header h1 {{ color: #4CAF50; }}
                        .code {{ font-size: 32px; font-weight: bold; color: #4CAF50; padding: 15px; background: #f0f8f0; border-radius: 5px; text-align: center; }}
                        .button {{ background: #4CAF50; color: white !important; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; }}
                        .footer {{ margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; color: #888; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🎓 Подтверждение регистрации</h1>
                        </div>
                        
                        <p>Привет, <strong>{name}</strong>!</p>
                        <p>Спасибо за регистрацию на платформе <strong>Teach</strong>.</p>
                        
                        <p>Ваш код подтверждения:</p>
                        <div class='code'>{code}</div>
                        
                        <br/>
                        
                        <div class='footer'>
                            <p>Код действителен <strong>5 минут</strong>.</p>
                            <p>Если вы не регистрировались, проигнорируйте это письмо.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }

        public static string GetWelcomeEmail(string name)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; background: #f5f5f5; margin: 0; padding: 20px; }}
                        .container {{ max-width: 600px; margin: 0 auto; background: white; border-radius: 8px; padding: 30px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                        .header h1 {{ color: #4CAF50; }}
                        .button {{ background: #4CAF50; color: white !important; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; }}
                        .footer {{ margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; color: #888; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🎉 Добро пожаловать в Teach!</h1>
                        </div>
                        
                        <p>Привет, <strong>{name}</strong>!</p>
                        <p>Мы рады видеть вас на нашей платформе.</p>
                        <p>Начните обучение прямо сейчас:</p>
                   
                        <div class='footer'>
                            <p>С уважением, команда Teach</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }


    }
}
