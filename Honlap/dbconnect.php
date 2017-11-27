            <?php
            function MySQLServerConnecter() {
                $servername = "sql11.freemysqlhosting.net:3306";
                $dbname= "sql11207393"; 
                $username = "sql11207393";                 
                $password = "CtdqJNMgAi";                 
                $conn = new mysqli($servername, $username, $password, $dbname);  
                
                if ($conn->connect_error) {
                    die("Connection failed: " . $conn->connect_error); 
                } else {
                    return $conn; 
                }
            }