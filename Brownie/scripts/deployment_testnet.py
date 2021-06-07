from brownie import accounts, BrownieWrap_Token, TokenHolderThresholdValidator, Faucet, Wei, network

def main():

    wltBadgerTeam = accounts.load('GasWallet')
    publishSource = False

    network.disconnect()
    network.connect('matic-test')
    print('----------------------------------------')
    print('Deploying Matic contracts. ')
    print('----------------------------------------')
    ctrBadgerLpToken = BrownieWrap_Token.deploy("Sett Vault Badger LP", "bBadger", {'from': wltBadgerTeam}, publish_source=publishSource)
    ctrBadgerHolderValidation = TokenHolderThresholdValidator.deploy(ctrBadgerLpToken.address, 10, 1, {'from': wltBadgerTeam}, publish_source=publishSource)

    network.disconnect()
    network.connect('bsc-test')
    print('----------------------------------------')
    print('Deploying Binance Smart Chain contracts.')
    print('----------------------------------------')
    ctrBadgerLpToken = BrownieWrap_Token.deploy("Sett Vault Badger LP", "bBadger", {'from': wltBadgerTeam}, publish_source=publishSource)
    ctrBadgerHolderValidation = TokenHolderThresholdValidator.deploy(ctrBadgerLpToken.address, 10, 1, {'from': wltBadgerTeam}, publish_source=publishSource)
    ctrFaucet = Faucet.deploy(10, Wei("50 gwei"), [], {'from' : wltBadgerTeam, 'value': Wei("500 gwei")}, publish_source=publishSource)

    print('Contract chain deployment complete.')