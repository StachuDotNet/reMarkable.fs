# this assumes you've already built, say, in an IDE

# copy
rsync -avh ./reMarkable.fs.Demo/bin/Debug/net5.0/ rM:/home/root/reMarkable.fs.Demo

# run
ssh rM "killall dotnet; systemctl stop xochitl; /opt/bin/rm2fb-client /home/root/dotnet/dotnet /home/root/reMarkable.fs.Demo/reMarkable.fs.Demo.dll"