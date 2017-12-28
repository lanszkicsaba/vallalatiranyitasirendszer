            <?php
            function MySQLServerConnecter() {
                $servername = "193.164.132.164:3306";
                $dbname= "csaba"; 
                $username = "csaba";                 
                $password = "DPU3wX9HYmGEL8HK";                 
                $conn = new mysqli($servername, $username, $password, $dbname);  
                
                if ($conn->connect_error) {
                    die("Connection failed: " . $conn->connect_error); 
                } else {
                    return $conn; 
                }
            }