#pragma warning(disable: 4786)

#include <iostream>
#include <string>
#include <vector>
#include <ctime>
//Limit the amount of responses
const int MAX_RESP = 30;
const std::string delim = "?!.;,";

typedef std::vector<std::string> vstring;

vstring find_match(std::string input);
void copy(char *array[], vstring &v);
void preprocess_input( std::string &str );
void UpperCase( std::string &str );
void cleanString( std::string &str );
bool isPunc(char c);

typedef struct {
    char *input;
    char *responses[MAX_RESP];
}record;

/*
 * Will store pre-generated responses that will answer basic questions that
 * the user is asking
 *
 * Once gets too big, consider replacing with an actual API
 */
record KnowledgeBase[] = {

};

size_t nKnowledgeBaseSize = sizeof(KnowledgeBase)/sizeof(KnowledgeBase[0]);

int main () {
    srand((unsigned) time(NULL));

    std::string sInput = "";
    std::string sResponse = "";
    std::string sPreviousResponse = "";
    std::string sPreviousInput = "";

    std::cout << "Type \"info\" for more info \n"; //remember to make path for code

    while(1) {
        std::cout << ">";
        sPreviousResponse = sResponse;
        sPreviousInput = sInput;
        std::getline(std::cin, sInput);
        preprocess_input(sInput);
        vstring responses = find_match(sInput);

        if (sInput == "INFO"){
            std::cout << "TEMPORARY INFO MESSAGE" << std::endl;
        } else if (sInput == sPreviousInput && sInput.length() > 0) {
            std::cout << "PLEASE DO NOT REPEAT YOURSELF" << std::endl;
        } else if (sInput == "BYE") {
            std::cout << "SEE YOU NEXT TIME" << std::endl;
            break;
        } else if (responses.size() == 0) {
            std::cout << "I DO NOT UNDERSTAND WHAT YOU'RE TALKING ABOUT, PLEASE TRY AGAIN" << std::endl;
        } else {
            int nSelection = rand() % responses.size();
            sResponse = responses[nSelection];

            if(sResponse == sPreviousResponse) {
                responses.erase(responses.begin() + nSelection);
                nSelection = rand() % responses.size();
                sResponse = responses[nSelection];
            }
            std::cout << sResponse << std::endl;
        }
    }

    return 0;
}

void preprocess_input( std::string &str ) {
    cleanString(str);
    UpperCase(str);
}

vstring find_match(std::string input) {
    vstring result;
    for (int i = 0; i < nKnowledgeBaseSize; ++i) {
        std::string keyWord(KnowledgeBase[i].input);

        if (input.find(keyWord) != std::string::npos) {
            copy(KnowledgeBase[i].responses, result);
            return result;
        }
    }
    return result;
}

void copy(char *array[],vstring &v) {
    for (int i = 0; i < MAX_RESP; ++i) {
        if (array[i] != NULL) {
            v.push_back(array[i]);
        } else {
            break;
        }
    }
}

void UpperCase( std::string &str ) {
    int len = str.length();
    for (int i = 0; i < len; i++) {
        if ( str[i] >= 'a' && str[i] <= 'z' ) {
            str[i] -='a' - 'A';
        }
    }
}

bool isPunc(char c) {
    return delim.find(c) != std::string::npos;
}

void cleanString( std::string &str ) {
    int len = str.length();
    std::string temp = "";

    char prevChar = 0;

    for (int i = 0; i < len; ++i) {
        if(str[i] == ' ' && prevChar == ' ') {
            continue;
        } else if(!isPunc(str[i])) {
            temp += str[i];
        }
        prevChar = str[i];
    }
    str = temp;
}
