<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>Bejelentkezés</title>
    </head>
    <body>
        <h1>Belépés</h1>
        <script>
            function validateForm() {
                var x = document.forms["login"]["username"].value;
                if (x == "") {
                    alert("Üres a Login név!");
                    return false;
                }
                var x = document.forms["login"]["psw"].value;
                if (x == "") {
                    alert("Üres a Jelszó!");
                    return false;
                }
            }
        </script>
        <?php
        echo '<form name="login" action="login_get.php" onsubmit="return validateForm()" method="post" enctype="multipart/form-data">
            Login név:<input type="text" name="username"><br>
            Jelszó:<input type="password" name="psw"><br>
            <input type="submit" value="Bejelentkezés" name="submit">
            </form>';
        echo '<h2>vagy</h2>';
        echo '<h2><a href="./regisztracio.php">Regisztráció</a></h2>';
        ?>
    </body>
</html>