
if [ "$1" == "--help" -o "$1" == "/?" ]
then
    echo ""
    echo "Updates the version number of the appveyor.yml build file under the root directory."
    echo "If a version number is not specified then it is deduced from the current branch with"
    echo "format (release|hotfix)/M.m.r"
    echo ""
    echo "Usage:"
    echo ""
    echo "bumpversion.sh [version]"
    echo ""
    echo "- <version> (optional) The version number to set, in the format M.m.r"
    echo ""
    echo "Note:"
    echo "The stored version number will have .{build} appended to the end (M.m.r.{build}),"
    echo "which allows the build system to replace it during builds"
    exit 0
fi

VERSION=$1
BRANCH=$(git branch | grep "*" | sed "s|* ||")
BRANCHTYPE=$(echo $BRANCH | sed "s|/.*||")

echo
echo "Branch type: '$BRANCHTYPE'"
echo "Branch name: '$BRANCH'"
echo


# If version number is not provided, deduce it from the branch name
if [ -e $VERSION ]
then
    # Ensure we are on a valid branch
    if [ $BRANCHTYPE != "release" -a $BRANCHTYPE != "hotfix" ]
    then
        echo -e "\e[1;31mError: Cannot auto-bump version on branches other than 'release' or 'hotfix'\e[0m"
        exit 1
    fi

    VERSION=$(echo $BRANCH | sed "s|.*/||")
fi

# Verify that the version number is in the correct format
if [ $(expr $VERSION : "[0-9]\+\.[0-9]\+\.[0-9]\+$") -eq 0 ]
then
    echo -e "\e[1;31mError: Incorrect version number format, expected '[0-9]\+\.[0-9]\+\.[0-9]\+$'\e[0m"
    exit 1
fi

echo "Updating version numbers to '$VERSION'"
echo


# The sed command below will create a temporary appveyor.tmp file with the version
# number replaced with the actual version numbers "version: 1.2.3.{build}"
sed -e "s|version: .*\.{build}|version: $VERSION\.{build}|" appveyor.yml > appveyor.tmp

if [ "$?" -ne "0" ]
then
    # Remove temporary file if it has been created
    rm -f appveyor.tmp

    echo
    echo -e "\e[1;31mError: Failed to create temporary appveyor.tmp file\e[0m"
    exit 1
fi

# Replace original appveyor.yml file with the temporary appveyor.tmp
mv appveyor.tmp appveyor.yml

if [ "$?" -ne "0" ]
then
    echo
    echo -e "\e[1;31mError: Failed to replace appveyor.yml file\e[0m"
    exit 1
fi


# Stage and commit files
git add appveyor.yml
git commit -m "Bumped version number to '$VERSION'" -q

if [ "$?" -ne "0" ]
then
    echo
    echo -e "\e[1;31mError: Failed to commit changes.\e[0m"
    exit 1
fi

echo -e "\e[1;32mSuccess: Version numbers updated to '$VERSION'\e[0m"


# NOTES:
# For further description of the 'sed' tool and how to use it see:
# http://devmanual.gentoo.org/tools-reference/sed/index.html
