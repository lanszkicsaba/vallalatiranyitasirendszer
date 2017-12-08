            <?php
            function MySQLServerConnecter() {
                $servername = "sql11.freemysqlhosting.net:3306";
                $dbname= "sql11209827"; 
                $username = "sql11209827";                 
                $password = "bIkIT755Ua";                 
                $conn = new mysqli($servername, $username, $password, $dbname);  
                
                if ($conn->connect_error) {
                    die("Connection failed: " . $conn->connect_error); 
                } else {
                    return $conn; 
                }
            }