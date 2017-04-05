import { ScreeningAutomationWebsitePage } from './app.po';

describe('screening-automation-website App', () => {
  let page: ScreeningAutomationWebsitePage;

  beforeEach(() => {
    page = new ScreeningAutomationWebsitePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
