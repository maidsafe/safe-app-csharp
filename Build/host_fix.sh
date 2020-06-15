echo "\nHosts before:"
cat /etc/hosts
echo "\nlocalhost:"
scutil --get LocalHostName
sudo -- sh -c "echo 127.0.0.1 $(scutil --get LocalHostName).local >> /etc/hosts"
printf "\nHosts after:"
cat /etc/hosts
