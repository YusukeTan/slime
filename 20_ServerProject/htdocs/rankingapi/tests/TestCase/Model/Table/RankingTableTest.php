<?php
namespace App\Test\TestCase\Model\Table;

use App\Model\Table\RankingTable;
use Cake\ORM\TableRegistry;
use Cake\TestSuite\TestCase;

/**
 * App\Model\Table\RankingTable Test Case
 */
class RankingTableTest extends TestCase
{

    /**
     * Test subject
     *
     * @var \App\Model\Table\RankingTable
     */
    public $Ranking;

    /**
     * Fixtures
     *
     * @var array
     */
    public $fixtures = [
        'app.ranking'
    ];

    /**
     * setUp method
     *
     * @return void
     */
    public function setUp()
    {
        parent::setUp();
        $config = TableRegistry::getTableLocator()->exists('Ranking') ? [] : ['className' => RankingTable::class];
        $this->Ranking = TableRegistry::getTableLocator()->get('Ranking', $config);
    }

    /**
     * tearDown method
     *
     * @return void
     */
    public function tearDown()
    {
        unset($this->Ranking);

        parent::tearDown();
    }

    /**
     * Test initialize method
     *
     * @return void
     */
    public function testInitialize()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }

    /**
     * Test validationDefault method
     *
     * @return void
     */
    public function testValidationDefault()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }
}
