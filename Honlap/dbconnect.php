            <?php
            function MySQLServerConnecter() {
                $servername = "sql11.freemysqlhosting.net:3306";
                $dbname= "sql11211489"; 
                $username = "sql11211489";                 
                $password = "X5vvu2Mbk8";                 
                $conn = new mysqli($servername, $username, $password, $dbname);  
                
                if ($conn->connect_error) {
                    die("Connection failed: " . $conn->connect_error); 
                } else {
                    return $conn; 
                }
            }