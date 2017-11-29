<?php

session_start();
?>
<?php

$_SESSION["login"] = "FALSE";
echo '<meta http-equiv="refresh" content="0; URL=index.php">';
