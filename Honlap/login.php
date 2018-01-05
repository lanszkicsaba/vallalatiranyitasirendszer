<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
        <title>Bejelentkezés</title>
    </head>
    <body>
		<div class="container">
			<div class="page-header">
				<h1>Belépés</h1>
			</div>
        <script>
            //ha valami üres
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
        //bejelentkezés form
        echo '<form class="form-horizontal" name="login" action="login_get.php" onsubmit="return validateForm()" method="post" enctype="multipart/form-data">
				<div class="form-group">
					<label class="control-label col-sm-2">Login név:</label>
					<div class="col-sm-2">
						<input type="text" class="form-control" name="username" placeholder="Felhasználónév"><br>
					</div>
				</div>
				<div class="form-group">
					<label class="control-label col-sm-2">Jelszó:</label>
					<div class="col-sm-2">
						<input type="password" class="form-control" name="psw" placeholder="Jelszó"><br>
					</div>
				</div>
				<div class="form-group">
					<div class="col-sm-offset-2 col-sm-10">
						<input type="submit" value="Bejelentkezés" class="btn btn-default" name="submit">
						<b>vagy</b>
						<a href="./regisztracio.php">Regisztráció</a>
					</div>
				</div>
				
            </form>';
        //visszagomb
			echo '<html>
				<form action="index.php" method="post">
				<input class="btn btn-default"  type="submit" value="Vissza"/>
				</form> 
			</html>';
		echo '</div>';
        ?>
    </body>
</html>