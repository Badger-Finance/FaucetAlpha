from brownie import accounts, BrownieWrap_Token, TokenHolderThresholdValidator, Faucet, Wei, network

def main():

    wltBadgerTeam = accounts[0]

    ctrBadgerLpToken = BrownieWrap_Token.deploy("Test", "TST", {'from': wltBadgerTeam})
    ctrBadgerHolderValidation = TokenHolderThresholdValidator.deploy(ctrBadgerLpToken.address, 10, 1, {'from': wltBadgerTeam})
    ctrFaucet = Faucet.deploy(10, Wei("50 gwei"), [], {'from' : wltBadgerTeam, 'value': Wei("500 gwei")})

    print('Contract chain deployment complete.')